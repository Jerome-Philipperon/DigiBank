import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import Account from 'src/app/Models/account';
import Saving from 'src/app/Models/saving';
import Deposit from 'src/app/Models/deposit';

@Component({
  selector: 'bank-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class DetailsComponent implements OnInit {
  @Input() account : Account;
  @Input() accountTransit : Saving;
  @Input() isSaving : boolean = false;
  @Input() accountTransit2 : Deposit;
  @Input() isDeposit : boolean = false;

  constructor(private route : ActivatedRoute, private http : HttpClient) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log(params);
      this.http.get(`https://localhost:44310/api/Clients/Account/${+params.id}`).subscribe((data : Account) => {
      this.account = data;
      this.defineAccountType();
    })
    })
  }

  defineAccountType ()
  {
    if(this.account.type == "Saving"){
      this.isSaving = true;
      this.accountTransit = <Saving> this.account as Saving;
      console.log(this.accountTransit.maximumDate)
    }
    else if(this.account.type == "Deposit") {
      this.isDeposit = true;
      this.accountTransit2 = <Deposit> this.account as Deposit;
    }
  }
}
