import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-box',
  templateUrl: './search.component.html',
  imports: [FormsModule]
})
export class SearchBoxComponent {
  @Input() placeholderText: string = 'Search...'; // Bahar se aayega
  @Input() value: string = ''; // Initial value agar chahiye toh

  @Output() textChanged = new EventEmitter<string>(); // Parent ko batane ke liye

  onSearchChange(newValue: string) {
    this.textChanged.emit(newValue); // Parent ke pass data jayega
  }
}