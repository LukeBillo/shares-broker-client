import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplaySharesComponent } from './display-shares.component';

describe('DisplaySharesComponent', () => {
  let component: DisplaySharesComponent;
  let fixture: ComponentFixture<DisplaySharesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DisplaySharesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DisplaySharesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
