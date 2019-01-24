import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { AuthenticateService } from '../authentication/authenticate.service';
import { User } from '../shared/user.model';
import { Share } from '../shared/share.model';
import { SharesService } from '../shares/shares.service';
import { share } from 'rxjs/operators';
import { UserSharesService } from '../shares/user-shares.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  searchCompanyName: string;

  private _user: User;
  private _topShares: Share[];
  private _searchedShares: Share[];
  private _ownedShares: Share[];

  public get user() {
    return this._user;
  }

  public get topShares() {
    return this._topShares;
  }

  public get searchedShares() {
    return this._searchedShares;
  }

  public get ownedShares() {
    return this._ownedShares;
  }

  public get ownedSharesTotal() {
    if (!this._ownedShares) {
      return 0;
    }

    const sharePrices = this._ownedShares.map(ownedShare => ownedShare.sharePrice.value * ownedShare.numberOfShares);
    return sharePrices.reduce((firstValue, secondValue) => firstValue + secondValue, 0);
  }

  constructor(
    private authenticateService: AuthenticateService,
    private sharesService: SharesService,
    private userSharesService: UserSharesService
  ) {}

  ngOnInit(): void {
    this._user = this.authenticateService.user;

    this.sharesService.getShares({}).subscribe(shares => {
      this._topShares = shares;
    });

    this.userSharesService.getUserShares(this._user.username).subscribe(userShares => {
      this._ownedShares = userShares;
    });
  }

  search(input: string) {
    if (input == null) {
      return;
    }

    this.sharesService.getShares({ companyName: input, limit: 5 }).subscribe(shares => {
      this._searchedShares = shares;
    });
  }
}
