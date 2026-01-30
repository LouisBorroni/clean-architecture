import { Component, OnInit, inject, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkDragDrop, DragDropModule, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { AuthService } from '../../core/services/auth.service';
import { TierlistService } from '../../core/services/tierlist.service';
import { Company } from '../../core/models/tierlist.models';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, DragDropModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @ViewChild('tierlistCapture') tierlistCapture!: ElementRef<HTMLElement>;

  authService = inject(AuthService);
  tierlistService = inject(TierlistService);

  isExporting = false;

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
    this.tierlistService.saveState();
  }

  async exportPdf(): Promise<void> {
    if (!this.tierlistCapture || this.isExporting) return;

    this.isExporting = true;

    try {
      const canvas = await html2canvas(this.tierlistCapture.nativeElement, {
        backgroundColor: '#ffffff',
        scale: 2,
        useCORS: true,
        allowTaint: true
      });

      const imageBase64 = canvas.toDataURL('image/png').split(',')[1];

      this.tierlistService.exportPdf(imageBase64).subscribe({
        next: (response) => {
          window.open(response.pdfUrl, '_blank');
          this.isExporting = false;
        },
        error: () => {
          alert('Erreur lors de l\'export du PDF');
          this.isExporting = false;
        }
      });
    } catch {
      alert('Erreur lors de la capture de la tier list');
      this.isExporting = false;
    }
  }
}
