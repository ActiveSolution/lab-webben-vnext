class PreviewImage {
    constructor(private url: string) { }

    public toHtml(): string {
        return `<div class="card">
                    <img src="${this.url}" class="img-fluid" />
                </div>`;
    }
}

class FileApplication {
    private filesInput: HTMLInputElement;
    private filesPreviewWrapper: HTMLDivElement;

    constructor() {
        this.filesInput = document.querySelector('#files') as HTMLInputElement;
        this.filesPreviewWrapper = document.querySelector('#files-preview') as HTMLDivElement;
    }

    public run(): void {
        if (this.filesInput !== undefined && this.filesPreviewWrapper !== undefined) {
            this.filesInput.addEventListener('change', event => {
                this.previewImages();
            });
        }
    }

    private previewImages(): void {
        this.filesPreviewWrapper.innerHTML = '';

        let files = this.filesInput.files;
        for (let file of Array.from(files)) {
            this.previewImage(file);
        }
    }

    private previewImage(file: File) {
        let fileReader = new FileReader();
        fileReader.addEventListener('load', () => {
            let image = new PreviewImage(fileReader.result);
            this.filesPreviewWrapper.innerHTML += image.toHtml();
        });
        fileReader.readAsDataURL(file);
    }
}

var app = new FileApplication();
app.run();