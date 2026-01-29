import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { AuthService } from '../../core/services/auth.service';
import { TierlistService } from '../../core/services/tierlist.service';
import { Company } from '../../core/models/tierlist.models';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, DragDropModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  authService = inject(AuthService);
  tierlistService = inject(TierlistService);

  ngOnInit(): void {
    this.tierlistService.loadCompanies();
  }

  logout(): void {
    this.authService.logout();
  }

  reset(): void {
    this.tierlistService.reset();
  }

  getConnectedLists(): string[] {
    const tierIds = this.tierlistService.tiers().map(t => `tier-${t.level}`);
    return ['unranked', ...tierIds];
  }

  drop(event: CdkDragDrop<Company[]>): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
    this.tierlistService.tiers.set([...this.tierlistService.tiers()]);
    this.tierlistService.unrankedCompanies.set([...this.tierlistService.unrankedCompanies()]);
  }
}
