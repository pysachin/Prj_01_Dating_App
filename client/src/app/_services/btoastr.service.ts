import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BtoastrService {

  private showSource = new ReplaySubject<boolean>(1);
  show$ = this.showSource.asObservable();

  private msgSource = new ReplaySubject<any>(1);
  msg$ = this.msgSource.asObservable();
  
  constructor() { }

  setShow(show: boolean, msg: any) {
    this.showSource.next(show);
    this.msgSource.next(msg);    
    setTimeout(()=>{

      this.showSource.next(false);
      this.msgSource.next('');   

    },2000);
  }

}
