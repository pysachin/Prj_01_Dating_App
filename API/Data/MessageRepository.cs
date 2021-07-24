using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext contex, IMapper mapper)
        {
            _context = contex;
            _mapper = mapper;
        }
        public async Task AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
             .Include(u => u.Sender)
             .Include(u => u.Recipient)
             .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                        .OrderByDescending(m => m.MessageSent)
                        .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username
                   && u.DateRead == null && u.RecipientDeleted == false
                )
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages,
            messageParams.PageNumber, messageParams.PageSize);

        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {

            var a = await _context.Messages.OrderBy(m => m.MessageSent)
                           .ToListAsync();
            var messages = await _context.Messages
                            .Include(u => u.Sender).ThenInclude(p => p.Photos)
                            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                            .Where(m => m.Recipient.UserName.ToLower() == currentUsername.ToLower()
                                && m.RecipientDeleted == false
                                && m.Sender.UserName.ToLower() == recipientUsername.ToLower()
                                || m.Recipient.UserName.ToLower() == recipientUsername.ToLower()
                                && m.Sender.UserName.ToLower() == currentUsername.ToLower()
                                && m.SenderDeleted == false
                            )
                            .OrderBy(m => m.MessageSent)
                            .ToListAsync();

            var unreadMessage = messages.Where(m => m.DateRead == null
                && m.Recipient.UserName == currentUsername).ToList();
            if (unreadMessage.Any())
            {
                foreach (var message in unreadMessage)
                {
                    message.DateRead = System.DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}