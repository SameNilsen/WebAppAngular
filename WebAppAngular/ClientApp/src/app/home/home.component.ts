import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ["./home.component.css"]
})
export class HomeComponent implements OnInit {

  slideIndex: number = 1;

  ngOnInit() {
    document.documentElement.setAttribute('data-bs-theme', "dark");

    this.showSlides(this.slideIndex);
  }

  //  Simple carousel stolen from: https://www.w3schools.com/howto/howto_js_slideshow.asp
  // Next/previous controls
  plusSlides(n: number): void {
    this.showSlides(this.slideIndex += n);
  }

// Thumbnail image controls
  currentSlide(n: number): void {
  this.showSlides(this.slideIndex = n);
}

  showSlides(n: number): void {
    let i;
      let slides = document.getElementsByClassName("mySlides") as HTMLCollectionOf<HTMLElement>;
    let dots = document.getElementsByClassName("dot");
    if (n > slides.length) { this.slideIndex = 1 }
    if (n < 1) { this.slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
      slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
      dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[this.slideIndex - 1].style.display = "block";
      dots[this.slideIndex - 1].className += " active";
  }
}
