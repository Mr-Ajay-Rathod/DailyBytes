import { Component, OnInit } from '@angular/core';
import { NewsService, Article } from './news.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-news-list',
  template: `
  <div *ngIf="items?.length; else empty">
    <div *ngFor="let it of items" class="card">
      <h2 (click)="open(it.id)" class="title">{{it.title}}</h2>
      <div class="meta">{{it.publishedAt | date:'medium'}}</div>
      <p>{{it.summary}}</p>
    </div>
  </div>
  <ng-template #empty>
    <p>No news available.</p>
  </ng-template>
  `,
  styles: [`.card{border:1px solid #ddd;padding:12px;margin-bottom:12px;border-radius:4px}.title{cursor:pointer;color:#007acc}`]
})
export class NewsListComponent implements OnInit {
  items: Article[] = [];
  constructor(private svc: NewsService, private router: Router) { }
  ngOnInit(): void {
    this.svc.list().subscribe(r => this.items = r);
  }
  open(id: number) { this.router.navigate(['/news', id]); }
}
