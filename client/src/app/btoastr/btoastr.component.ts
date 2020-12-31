import { Component, OnInit } from '@angular/core';
import { BtoastrService } from '../_services/btoastr.service';

@Component({
  selector: 'app-btoastr',
  templateUrl: './btoastr.component.html',
  styleUrls: ['./btoastr.component.css']
})
export class BtoastrComponent implements OnInit {

  constructor(public btoastr : BtoastrService) { }

  ngOnInit(): void {
    this.btoastr.setShow(false,'');    
  }

}
