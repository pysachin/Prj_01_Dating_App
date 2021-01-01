import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {

  baseUrl = "https://localhost:5001/api/";
  validationErrors : string[] = [];
  constructor(private http : HttpClient) { }

  ngOnInit(): void {
  }

  get500Error()
  {
      this.http.get(this.baseUrl + 'buggy/server-error').subscribe(
        r =>{
          console.log(r);
        },e => {
          console.log(e);
        }
      )
  }

  get401Error()
  {
      this.http.get(this.baseUrl + 'buggy/auth').subscribe(
        r =>{
          console.log(r);
        },e => {
          console.log(e);
        }
      )
  }

  get404Error()
  {
      this.http.get(this.baseUrl + 'buggy/not-found').subscribe(
        r =>{
          console.log(r);
        },e => {
          console.log(e);
        }
      )
  }

  get400Error()
  {
      this.http.get(this.baseUrl + 'buggy/bad-request').subscribe(
        r =>{
          console.log(r);
        },e => {
          console.log(e);
        }
      )
  }

  post400Error()
  {
      this.http.post(this.baseUrl + 'account/register',{}).subscribe(
        r =>{
          console.log(r);
        },e => {
          console.log(e);
          this.validationErrors = e;
        }
      )
  }

}
