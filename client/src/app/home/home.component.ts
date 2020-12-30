import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode: boolean = false;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {

  }

  toggleRegisterMode() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(val: boolean) {
    this.registerMode = val;
  }

}
