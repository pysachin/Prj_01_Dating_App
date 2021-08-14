import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_model/User';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: Partial<User[]>;
  bsModalRef: BsModalRef;

  constructor(private adminService:AdminService,
    private modalService:BsModalService) { }

  ngOnInit(): void {
    this.getUserWithRoles();
  }

  getUserWithRoles(){
    this.adminService.getUserWithRoles()
    .subscribe(users => {
      this.users = users;
    });
    
  }

  openRolesModal(user:any){
    
    const config = {
      class:'modal-dialog-centered',
      initialState:{
        user,
        roles: this.getRolesArray(user)
      }      
    }

    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe(values =>{
        const rolesToUpdate = {
          roles: [...values.filter(el => el.checked === true).map(el => el.name)]
        };
        if(rolesToUpdate) {          
            this.adminService
            .updateUserRoles(user.username,rolesToUpdate.roles)
            .subscribe(()=>{
               user.roles = [...rolesToUpdate.roles]
            });
        }
    });
    this.bsModalRef.content.closeBtnName = 'Close';

  }

  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
        {name: 'Admin',value:'Admin'},
        {name: 'Moderator',value:'Moderator'},
        {name: 'Member',value:'Member'},
    ];
  
    availableRoles.forEach( r =>{
        
      let isMatch = false;

        for(const userRole of userRoles) {         
          if(r.name === userRole){
            isMatch= true;
            r.checked = true;
            roles.push(r);
            break;
          }
        }
        
        if(!isMatch) {
          r.checked = false;
          roles.push(r);
        }
    });
   
    return roles;
  }

}
