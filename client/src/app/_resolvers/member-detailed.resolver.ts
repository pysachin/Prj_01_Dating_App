import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Member } from "../_model/Members";
import { MembersService } from "../_services/members.service";

@Injectable({
    providedIn:'root'
})
export class MemberDetailedResolver implements Resolve<Member> {

    /**
     *
     */
    constructor(private memberService: MembersService ) {        

    }

    resolve(route: ActivatedRouteSnapshot):Observable<Member>{
        return this.memberService.getMember(route.paramMap.get('username'));
    }

}