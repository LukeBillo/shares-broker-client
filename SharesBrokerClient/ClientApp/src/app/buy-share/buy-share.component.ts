import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SharesService, BuyShareRequest } from '../shares/shares.service';
import { AuthenticateService } from '../authentication/authenticate.service';
import { User } from '../shared/user.model';
import { Share } from '../shared/share.model';

@Component({
  selector: 'app-buy-share',
  templateUrl: './buy-share.component.html',
  styleUrls: ['./buy-share.component.css'],
})
export class BuyShareComponent implements OnInit {
  pending = false;
  buyForm: FormGroup;
  shareToBuy: string;
  companyShareBeingBought: Share;
  private _user: User;

  get buyingShare() {
    return this.buyForm.controls.buyingShare;
  }

  get numberOfSharesToBuy() {
    return this.buyForm.controls.numberOfShares;
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private sharesService: SharesService,
    authenticateService: AuthenticateService
  ) {
    this._user = authenticateService.user;
  }

  ngOnInit() {
    this.buyForm = this.formBuilder.group({
      buyingShare: ['', Validators.required],
      numberOfShares: ['', Validators.required],
    });

    this.buyForm.get('buyingShare').valueChanges.subscribe(buyingShare => {
      this.shareToBuy = buyingShare;

      if (buyingShare == null) {
        return;
      }

      this.sharesService.getShares({ companySymbol: buyingShare, limit: 1 }).subscribe(share => {
        this.companyShareBeingBought = share[0];
      });
    });

    this.route.queryParamMap.subscribe(params => {
      this.buyForm.get('buyingShare').setValue(params.get('share'));
    });
  }

  buy() {
    const buyRequest: BuyShareRequest = {
      buyerUsername: this._user.username,
      companySymbol: this.buyingShare.value,
      numberOfSharesToBuy: this.numberOfSharesToBuy.value,
    };

    this.sharesService.buyShare(buyRequest).subscribe(_ => {
      this.router.navigate(['']);
    });
  }
}
