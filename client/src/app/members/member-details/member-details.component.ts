import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_model/Members';
import { MembersService } from 'src/app/_services/members.service';
import {NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {NgxGalleryImage} from '@kolkov/ngx-gallery';
import {NgxGalleryAnimation} from '@kolkov/ngx-gallery';
import { getAttrsForDirectiveMatching } from '@angular/compiler/src/render3/view/util';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_model/Message';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {

  @ViewChild('memberTabset', {static: true}) memberTabset : TabsetComponent;
  member:Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab : TabDirective;
  messages: Message[] = [];

  constructor(private memberService:MembersService,
    private route: ActivatedRoute,
    private messageService: MessageService
    ) { }

  ngOnInit(): void {
    //this.loadMember();
    this.route.data.subscribe(data => {
      this.member = data.member;
    });


    this.route.queryParams.subscribe(params =>{
       params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];   

    this.galleryImages = this.getImages();

  }

  getImages(): NgxGalleryImage[]{
    const imageUrls = [];
    for (const photo of this.member.photos) {

      imageUrls.push({
        small:photo?.url,
        medium:photo?.url,
        big:photo?.url
      })          
    }
    return imageUrls;
}

  // loadMember(){
  //   this.memberService
  //   .getMember(this.route.snapshot.paramMap.get('username'))
  //   .pipe(take(1))
  //   .subscribe(member => {
  //     this.member = member;
     
  //   })   

  // }

  loadMessages(){  
    this.messageService.getMessageThread(this.member.userName)
      .subscribe(messages => {
         this.messages = messages;
      });
  }

  selectTab(tabId: number){
    this.memberTabset.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective){
      this.activeTab = data;
      if(this.activeTab.heading === 'Messages' && this.messages.length === 0)
      {
         this.loadMessages();
      }
  }


}
