import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Options } from 'ng5-slider';
import { SliderComponent } from 'ng5-slider/slider.component';
import { Currency } from '../shared/currency.enum';
import { SharesService } from '../shares/shares.service';
import { SharesQuery } from '../shared/sharesQuery.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  searchForm: FormGroup;

  enablePriceRange = false;
  priceRangeSliderOptions: Options;

  priceLowerValue = 0;
  priceUpperValue = 30000;

  currencies: Array<String> = [];
  currencyDropdownConfig = {
    placeholder: 'GBP',
    height: '200px',
    search: true
  };

  limitSliderOptions: Options;

  constructor(private formBuilder: FormBuilder, private sharesService: SharesService, private router: Router) {
    for (const currency in Currency) {
      if (Currency.hasOwnProperty(currency)) {
        this.currencies.push(currency);
      }
    }
  }

  ngOnInit() {
    this.searchForm = this.formBuilder.group({
      companySymbol: [''],
      companyName: [''],
      currency: [''],
      priceBetween: [''],
      lessThanSharesAvailable: [''],
      moreThanSharesAvailable: [''],
      limit: ['']
    });

    this.priceRangeSliderOptions = {
      floor: 0,
      step: 1000,
      ceil: 300000,
      disabled: !this.enablePriceRange
    };

    this.limitSliderOptions = {
      floor: 1,
      step: 1,
      ceil: 50
    };
  }

  enablePriceRangeChanged(_) {
    this.priceRangeSliderOptions = {
      ...this.priceRangeSliderOptions,
      disabled: !this.enablePriceRange
    };
  }

  search() {
    if (this.formIsEmpty()) {
      return;
    }

    this.router.navigate(['/display'], { queryParams: this.constructSharesQuery() });
  }

  private formIsEmpty() {
    const searchForm = this.searchForm.controls;

    for (const field in searchForm) {
      if (searchForm.hasOwnProperty(field)) {
        if (searchForm[field] != null) {
          return false;
        }
      }
    }

    return true;
  }

  private constructSharesQuery(): Partial<SharesQuery> {
    const sharesQuery: Partial<SharesQuery> = {};
    const searchForm = this.searchForm.controls;

    if (searchForm.companyName.value) {
      sharesQuery.companyName = searchForm.companyName.value;
    }

    if (searchForm.companySymbol.value) {
      sharesQuery.companySymbol = searchForm.companySymbol.value;
    }

    if (searchForm.currency.value) {
      sharesQuery.currency = searchForm.currency.value;
    }

    if (searchForm.priceBetween.value && this.enablePriceRange) {
      sharesQuery.priceLessThan = searchForm.priceBetween.value[0];
      sharesQuery.priceMoreThan = searchForm.priceBetween.value[1];
    }

    if (searchForm.lessThanSharesAvailable.value) {
      sharesQuery.availableSharesLessThan = searchForm.lessThanSharesAvailable.value;
    }

    if (searchForm.moreThanSharesAvailable.value) {
      sharesQuery.availableSharesMoreThan = searchForm.moreThanSharesAvailable.value;
    }

    if (searchForm.limit.value) {
      sharesQuery.limit = searchForm.limit.value;
    }

    return sharesQuery;
  }
}
