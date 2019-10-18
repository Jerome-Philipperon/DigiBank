import  Account  from './account';

export default class Client {
    firstName : string;
    lastName : string;
    dateOfBirth : Date;
    street : string;
    zipCode : number;
    city : string;
    myAccounts : Account[];
    email : string;
}
