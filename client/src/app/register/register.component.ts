import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../_services/account.service';
import { BtoastrService } from '../_services/btoastr.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {


  @Output() cancelRegisterMode = new EventEmitter();

  model: any = {}

  constructor(private accountService: AccountService,
    public btoastr : BtoastrService  
    ) { }

  ngOnInit(): void {

  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancel();
    }, error => 
    { 
      this.btoastr.setShow(true,error.error);
      console.log(error);      
    })
  }

  cancel() {
    this.cancelRegisterMode.emit(false);
  }

}
