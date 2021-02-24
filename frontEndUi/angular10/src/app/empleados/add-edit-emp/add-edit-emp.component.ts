import { Component, OnInit, Input } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-add-edit-emp',
  templateUrl: './add-edit-emp.component.html',
  styleUrls: ['./add-edit-emp.component.css']
})

export class AddEditEmpComponent implements OnInit {

  form: FormGroup;
  id: string;
  isAddMode: boolean;
  loading = false;
  submitted = false;

  constructor(
    private service:SharedService, 
    private formBuilder: FormBuilder
  ) {}

  @Input() emp:any;
  Id:string;
  Nombre:String;
  Telefono:string;
  Direccion:string;
  Curp:string;
  Imagen:string;
  PathImagen:string;

  ngOnInit(): void {
    this.isAddMode = (this.emp.Id==0)? true : false;

    this.form = this.formBuilder.group({
      Nombre: [ '', Validators.required],
      Telefono: ['', Validators.required,Validators.pattern('^[0-9\-\+]{9,15}$')],
      Direccion: ['', Validators.required],
      Curp: ['', [Validators.required, Validators.pattern('^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$')]]
    });
    
      this.Id = this.emp.Id;
      this.Direccion = this.emp.Direccion;
      this.Nombre = this.emp.Nombre;
      this.Telefono = this.emp.Telefono;
      this.Curp = this.emp.Curp;
      this.Imagen = this.emp.Imagen;
      this.PathImagen = this.service.PhotoUrl + this.emp.Imagen;

      this.form['Nombre'] = this.Nombre;
      this.form['Telefono'] = this.Telefono;
      this.form['Direccion'] = this.Direccion;
      this.form['Curp'] = this.Curp;
  }

    get f() { return this.form.controls; }

    onSubmit() {
      this.submitted = true;
      
      alert(this.isAddMode);

      // reset alerts on submit
      //this.alertService.clear();
      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }

      this.loading = true;
      if (this.isAddMode) {
        alert("add");
          this.addEmpleado();
      } else {
        alert("update");
          this.updateEmpleado();
      }
  }

  addEmpleado(){
      var val ={
        Id:this.Id,
        Nombre:this.Nombre,
        Telefono:this.Telefono,
        Direccion:this.Direccion,
        Curp:this.Curp,
        Imagen:this.Imagen};
        this.service.addEmpleado(val).subscribe(res=>{
          alert(res.Message);
        });
        this.loading = false;
    }

  updateEmpleado(){
      var val ={
        Id:this.Id,
        Nombre:this.Nombre,
        Telefono:this.Telefono,
        Direccion:this.Direccion,
        Curp:this.Curp,
        Imagen:this.Imagen};
        this.service.updateEmpleado(val).subscribe(res=>{
          alert(res.Message);
      });
      this.loading = false;
  }
  uploadPhoto(event){
    var file = event.target.files[0];
    const formData:FormData = new FormData();
    formData.append('uploadFile',file,file.name);

    this.service.UploadPhoto(formData).subscribe((data:any)=>{
      this.Imagen = data.toString();
      this.PathImagen = this.service.PhotoUrl + data.toString();
      alert( this.PathImagen);
    })
  }

  imprimirGafete(){
    
  }

}
