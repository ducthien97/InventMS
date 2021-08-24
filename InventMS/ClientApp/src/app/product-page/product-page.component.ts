import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Inject } from '@angular/core';

@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
  styleUrls: ['./product-page.component.css']
})
export class ProductPageComponent implements OnInit {
  public products: Product[];
  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Product[]>(baseUrl + 'products').subscribe(result => {
      this.products = result;
    }, error => console.error(error));
  }
  ngOnInit() {
  }

}
interface Category {
  id: number;
  name: string;
}

interface Manufacturer {
  id: number;
  name: string;
  country: string;
}

interface Product {
  id: number;
  name: string;
  msrPrice: number;
  quantity: number;
  customerRating: number;

  category: Category;
  manufacturer: Manufacturer;
}