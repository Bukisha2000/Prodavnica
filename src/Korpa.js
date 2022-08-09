import React from 'react'
import { useEffect } from 'react'
import { useState } from 'react'
import { useSelector, useDispatch } from "react-redux";
import { removeOrder } from './OrderSlice';

export default function Korpa() {
    var change =  (JSON.parse(localStorage.getItem('order')));
    const [pokreniRender,SetPokreniRender] = useState(false);
    const [proizvodi,SetProizvodi] = useState([]);
    const [totalPrice,SetTotalPrice] = useState(0);
    const [kredencijali,SetKredencijali] = useState(0);
    const[korisnik,SetKorisnik] = useState("");
    const [azurirajProizvode,SetAzuriranjeProizvoda] = useState(false);
  
    const dispatch = useDispatch();
   
    useEffect(() => {
    
        change = (JSON.parse(localStorage.getItem('order')));
        
        let url = "";
        let counter = 0;
        let goodUrl = 'https://localhost:7142/api/Prodavnica/byID?'
        if(change !== null){
          for(let a = 0; a<Object.keys(change).length; a++){
            counter++;
            if(counter == Object.keys(change).length){
                url += 'idProd='+Object.keys(change)[a];
            }else{
                url+= 'idProd='+Object.keys(change)[a] + '&';
                
            }
            
            
        }
        }
       
        if(url !== ""){
          goodUrl+=url;
          fetch(goodUrl, {
              method: "GET",
              headers: {
                "Content-Type": "application/json",
                Accept: "application/json",
              },
            })
              .then((response) => response.json())
              .then((responseFromServer) => {
             SetProizvodi(responseFromServer);
            
              })
              .catch((error) => {
                console.log(error);
              });
        }
       
        if(change == null || Object.keys(change).length == 0){
          var helperArray = [];
          SetProizvodi(helperArray);
          // BUG kada stavim change na {} prazan objekat, on ce mi ostaviti jednu jedinicu u korpi i nece je izbrisati,
          // Ovako cu mu reci, ako je change prazan, stavi mi da lista proizvoda u korpi bude praznam, i izbrisace sve iz korpe
        }
   
        var myUrl = "https://localhost:7142/api/Korisnik/me"
      fetch(myUrl, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Accept: "application/json",
          'Authorization': "bearer" + " " + JSON.parse(localStorage.getItem('token'))
        },
      })
        .then((response) => response.json())
        .then((responseFromServer) => {
      SetKredencijali(responseFromServer.serNumber);
      SetKorisnik(responseFromServer.userName)
      
        })
        .catch((error) => {
          console.log(error);
        });
      
       
       
    }, [pokreniRender]);

    useEffect(() => {
     SetTotalPrice(0);
     var singlePrice = 0;
    
      if(proizvodi.length > 0){
       
        for(let a = 0; a< proizvodi.length; a++){
     
          singlePrice += proizvodi[a].productPrice*change[proizvodi[a].id];
       
        }
        SetTotalPrice(singlePrice);
      }
      
    }, [proizvodi]);

 useEffect(() => {
  var order = JSON.parse(localStorage.getItem('order'));
  var dodatakURL = "";
  var Moj_URL = "https://localhost:7142/api/Prodavnica/kupovina?"
 
  if(azurirajProizvode == true){
   for(let a = 0; a<Object.keys(order).length; a++){
    if(dodatakURL !== ""){
      dodatakURL += "&idProd=" + Object.keys(order)[a] + "&";
    }else{
      dodatakURL += "idProd=" + Object.keys(order)[a] + "&";
    }
    dodatakURL += "kolicina=" + order[Object.keys(order)[a]];
 
   }
   Moj_URL += dodatakURL;
  console.log(Moj_URL);
    fetch(Moj_URL, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    })
      .then((response) => response.json())
      .then((responseFromServer) => {
       
   })
      
      .catch((error) => {
        console.log(error);
      });
      localStorage.removeItem('order');
  }
 
 },[azurirajProizvode])
    function handleDeleteFromCart(e){
      localStorage.removeItem('order');
        delete change[e.target.value];
        dispatch(removeOrder({id: e.target.value}));
       
        localStorage.setItem('order',JSON.stringify(change));
       
        if(pokreniRender == true){
            SetPokreniRender(false);
            // inicijalni state je false cisto onako, i ovde govorimo ako je state falsse stavi mi true i obrnuto, cisto da bi azurirali..
        }else{
            SetPokreniRender(true);
        }
    }

    function handlePlacanjeKarticom(){
     
      var url = 'https://localhost:7142/Skidanje?'

      var kartica = 'brojKartice='+ kredencijali;
      var iznos = '&iznos=' + totalPrice;

     
     const konacno = url+kartica+iznos;
    
      fetch(konacno, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Accept: "application/json",
        },
      })
        .then((response) => response.json())
        .then((responseFromServer) => {
   
     if(responseFromServer == "Uspesna kupovina!"){
      SetAzuriranjeProizvoda(false);
      SetAzuriranjeProizvoda(true);
      
     }
     alert(responseFromServer + korisnik);
  

      
        })
        .catch((error) => {
          console.log(error);
        });
        
    }
   
  return (
    <div>
        <div className='proizvodiDiv'>
            {proizvodi.map((jedan) => (
            <div className='jedanProizvodDiv' key={jedan.id}>
        <h3>{jedan.productName}</h3>
          <div className='slikaProizvod'>
          <img src={require("./" + jedan.productPicture)} />
        
          </div>
          <h4>Cena: <span>{jedan.productPrice}</span></h4>
          <h4>Izabrano: <span>{change[jedan.id]}</span></h4>
          <h4>Ukupna cena: <span>{change[jedan.id] * jedan.productPrice}</span></h4>
          <button onClick={handleDeleteFromCart} value={jedan.id}>Obrisi iz korpe!</button>
        </div>
            ))}
        
        </div>
        <h4>Ukupno: {totalPrice}</h4>
        <button onClick={handlePlacanjeKarticom}>Plati</button>
    </div>
  )
}
