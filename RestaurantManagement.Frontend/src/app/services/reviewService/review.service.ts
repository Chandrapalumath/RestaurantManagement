import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { CustomerResponse, ReviewCreateRequest, ReviewResponse } from "../../models/review.model";

@Injectable({ providedIn: 'root' })
export class ReviewService {
  private http = inject(HttpClient);
  private readonly reviewApi = 'https://localhost:7095/api/reviews';
  private readonly customerApi = 'https://localhost:7095/api/customers';

  createRestaurantReview(data: ReviewCreateRequest) {
    return this.http.post(this.reviewApi, data);
  }

  getReviews() {
    return this.http.get<ReviewResponse[]>(this.reviewApi);
  }

  getCustomers() {
    return this.http.get<CustomerResponse[]>(this.customerApi);
  }
}