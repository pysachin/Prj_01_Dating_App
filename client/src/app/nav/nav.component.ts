import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_model/User';

import { AccountService } from '../_services/account.service';
import { BtoastrService } from '../_services/btoastr.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {}

  constructor(public accountService: AccountService, 
    private router: Router,
    public btoastr : ToastrService    
    ) { }

  ngOnInit(): void {

  }

  login() {
    console.log(this.model);
    this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');      
      console.log(response);
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }



}
