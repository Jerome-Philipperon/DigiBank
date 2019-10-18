import Client from './client';

export default class Account {
    bankCode : string;
    branchCode : string;
    accountNumber : string;
    Key : string;
    IBAN : string;
    BIC : string;
    balance : number;
    AccountOwner : Client;
}
