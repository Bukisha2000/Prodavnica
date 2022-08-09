import React from 'react'
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Login() {
const [userLogin,SetUserLogin] = useState('');
const [error,SetError] = useState('');
const [showError,SetShowError] = useState(false);
const navigate = useNavigate();
    function Login() {
          
        let url = `https://localhost:7142/api/Korisnik/login?KorisnikIme=${userLogin.KorisnikIme}&korSifra=${userLogin.korSifra}`

        fetch(url, {
            method: 'GET',
            headers : { 
               'Content-Type': 'application/json',
               'Accept': 'application/json'
              }
        })
            .then(response => response.json())
            .then(responseFromServer => {
                if(responseFromServer == "Ne postoji korisnik sa ovim imenom!" || responseFromServer == "Neispravna Sifra!"){
                    SetError(responseFromServer);
                    SetShowError(true);
                }else{
                    console.log(responseFromServer);
                    localStorage.setItem('token',JSON.stringify(responseFromServer));
                    navigate('/izbor');
                  
                }
            })
            .catch((error) => {
               
                console.log(error);
            })
         
          }
          function HandleLoginName(e){
            SetUserLogin({
                ...userLogin,
                KorisnikIme: e.target.value,
            });
          }
          function HandleLoginPass(e){
            SetUserLogin({
                ...userLogin,
                korSifra: e.target.value,
            });
          }
  return (
    <>
  
    <div className='LoginDiv'>
        <h4>Ime korisnika:</h4>
        <input  onChange={HandleLoginName}></input>
        <h4>Sifra korisnika:</h4>
        <input  onChange={HandleLoginPass}></input>
        <button onClick={Login}>Login</button>
        {showError && <h4>{error}</h4>}
        

    </div>

  
   </>
  )
}
