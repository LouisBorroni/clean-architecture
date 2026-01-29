import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent {
  tiers = [
    { label: 'S', color: '#22c55e' },
    { label: 'A', color: '#84cc16' },
    { label: 'B', color: '#eab308' },
    { label: 'C', color: '#f97316' },
    { label: 'D', color: '#6b7280' }
  ];
}
