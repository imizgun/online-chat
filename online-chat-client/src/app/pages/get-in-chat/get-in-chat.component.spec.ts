import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetInChatComponent } from './get-in-chat.component';

describe('GetInChatComponent', () => {
  let component: GetInChatComponent;
  let fixture: ComponentFixture<GetInChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetInChatComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetInChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
