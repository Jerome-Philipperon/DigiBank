import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CurrentUserService } from '../current-user.service';
import Client from '../Models/client';

@Component({
  selector: 'app-informations',
  templateUrl: './informations.component.html',
  styleUrls: ['./informations.component.css']
})
export class InformationsComponent implements OnInit {

 @Input() myClient:Client;

  constructor(private currentUser : CurrentUserService) { }

  ngOnInit() {
    this.myClient = this.currentUser.getCurrentUser();
  }

}
