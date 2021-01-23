import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import {Member} from 'src/app/_model/Members'

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl:string = environment.apiUrl;
  constructor(private http:HttpClient) { 
    
  }

  getMembers(): Observable<Member[]>{
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username : string): Observable<Member>{
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

}
