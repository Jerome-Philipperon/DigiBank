import Account from './account';

export default class Saving extends Account{
    minimumAmount : number;
    maximumAmount : number;
    interestRate : number;
    maximumDate : Date;
}
