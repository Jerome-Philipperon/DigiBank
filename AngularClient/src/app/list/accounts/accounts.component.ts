import { Component, OnInit, Input } from '@angular/core';
import Account from '../../Models/account';
import { Router } from '@angular/router';

@Component({
  selector: 'bank-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit {
  @Input() account : Account;

  constructor(private router : Router) { }

  ngOnInit() {
  }

  showDetails ()
  {
    this.router.navigate(['accounts/details', this.account.accountId]);
  }
}
