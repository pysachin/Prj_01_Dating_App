import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { BtoastrService } from '../_services/btoastr.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService : AccountService, 
    private btoastr : BtoastrService,
    private router: Router){

  }

  canActivate(): Observable<boolean> {

    return this.accountService.currentUser$.pipe(
      map(user =>{

        if(user) 
        {
          return true;
        }
        else
        {
          this.btoastr.setShow(true,'Please Loging !');
          this.router.navigateByUrl('/');
          return false;          
        }
        
      })      
    );
    
  }
  
}
