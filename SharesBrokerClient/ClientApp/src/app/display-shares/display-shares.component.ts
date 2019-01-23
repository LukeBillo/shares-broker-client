import { Component, OnInit } from '@angular/core';
import { SharesService } from '../shares/shares.service';
import { ActivatedRoute } from '@angular/router';
import { SharesQuery } from '../shared/sharesQuery.model';
import { Share } from '../shared/share.model';

@Component({
  selector: 'app-display-shares',
  templateUrl: './display-shares.component.html',
  styleUrls: ['./display-shares.component.css']
})
export class DisplaySharesComponent implements OnInit {
  private sharesQuery: Partial<SharesQuery>;
  private _sharesFound: Share[];
  limit = 10;

  public get sharesFound() {
    return this._sharesFound;
  }

  constructor(private route: ActivatedRoute, private sharesService: SharesService) {
    this.route.queryParamMap.subscribe(params => {
      this.sharesQuery = params as Partial<SharesQuery>;

      if (this.sharesQuery.limit) {
        this.limit = this.sharesQuery.limit;
      }

      this.sharesService.getShares(this.sharesQuery).subscribe(shares => {
        this._sharesFound = shares;
      });
    });
  }

  ngOnInit() {}
}
