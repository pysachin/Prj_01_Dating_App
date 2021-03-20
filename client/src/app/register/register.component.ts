import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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

  registerForm:FormGroup;
  model: any = {}
  maxDate:Date;
  validationErrors:string[]=[];

  constructor(private accountService: AccountService,
    public btoastr : ToastrService,
    private fb : FormBuilder ,
    private router : Router 
    ) { }

  ngOnInit(): void {
    this.intializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
  }

  intializeForm(){
    this.registerForm = this.fb.group({
      gender :['male'],
      username :['',Validators.required],
      knownAs :['',Validators.required],
      dateOfBirth :['',Validators.required],
      city :['',Validators.required],
      country :['',Validators.required],
      password :['',[Validators.required,
      Validators.minLength(4),Validators.maxLength(8)
      ]],
      confirmPassword :['',
      [Validators.required,this.matchValues('password')]],
    });
  }

  matchValues(matchTo:string):ValidatorFn{
    return (control : AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching : true};
    }
  }

  register() {
   
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, error => 
    {       
      //this.btoastr.error(error.error)
      this.validationErrors = error;
         
    })
  }

  cancel() {
    this.cancelRegisterMode.emit(false);
  }

}
