import { Injectable } from '@angular/core';
import { CanActivate, CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements  CanDeactivate<unknown> {


  constructor(private confirmService:ConfirmService){}

  canDeactivate(component: MemberEditComponent) : 
  Observable<boolean> | boolean {
        
    if(component.tmpEditForm && component.tmpEditForm.dirty){
        return this.confirmService.confirm();
    }
    return true;
  }
  
}
