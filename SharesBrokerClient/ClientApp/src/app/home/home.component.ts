import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { AuthenticateService } from '../authentication/authenticate.service';
import { User } from '../shared/user.model';
import { Share } from '../shared/share.model';
import { SharesService } from '../shares/shares.service';
import { share } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  searchCompanyName: string;

  private _user: User;
  private _topShares: Share[];
  private _searchedShares: Share[];

  public get user() {
    return this._user;
  }

  public get topShares() {
    return this._topShares;
  }

  public get searchedShares() {
    return this._searchedShares;
  }

  constructor(private authenticateService: AuthenticateService, private sharesService: SharesService) {}

  ngOnInit(): void {
    this._user = this.authenticateService.user;

    this.sharesService.getShares({}).subscribe(shares => {
      this._topShares = shares;
    });
  }

  search(input: string) {
    if (input == null) {
      return;
    }

    this.sharesService.getShares({ companyName: input }).subscribe(shares => {
      this._searchedShares = shares;
    });
  }
}
