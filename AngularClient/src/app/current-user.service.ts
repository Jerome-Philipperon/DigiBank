import { Injectable } from '@angular/core';
import Client from './Models/client';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  currentUser : Client;

  constructor() { }

  public getCurrentUser ()
  {
    return this.currentUser;
  }

  public setCurrentUser (user : Client)
  {
    this.currentUser = user;
  }
}
