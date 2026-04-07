import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Article {
  articleId: number;
  headLine: string;
  subHeading: string;
  content: string;
  createdDate: string;
  categoryId: number;
  category?: { categoryId: number; categoryName: string };
}

@Injectable({ providedIn: 'root' })
export class NewsService {
  private base = '/api/articles';
  constructor(private http: HttpClient) { }

  list(): Observable<Article[]> {
    return this.http.get<Article[]>(this.base);
  }

  get(id: number) {
    return this.http.get<Article>(`${this.base}/${id}`);
  }
}
