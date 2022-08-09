import React from "react";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { addOrder } from "./OrderSlice";
export default function Prodavnica() {
  const navigate = useNavigate();
  const [proizvodi, SetProizvodi] = useState([]);
  const [ukupnoDodano, SetUkupnoDodano] = useState(0);

  const dispatch = useDispatch();
  const brojacOrdera = useSelector((state) => state.orders);

  function navigateLogin() {
    localStorage.clear();
    navigate("/");
  }
  useEffect(() => {
    const url = "https://localhost:7142/api/Prodavnica";
    fetch(url, {
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
  }, []);

  function handleOrder(e) {
    let ukupnoStanje = parseInt(
      document.getElementById(e.target.value).textContent
    );
    if (Number.isInteger(ukupnoDodano) || ukupnoDodano === 0) {
      alert("niste izabrali kolicinu!");
      return;
    }
    if (ukupnoStanje < ukupnoDodano) {
      return alert("Nema dostupno toliko na stanju!");
    } else {
      dispatch(
        addOrder({
          id: e.target.value,
          brojac: ukupnoDodano,
          stanje: ukupnoStanje,
        })
      );
    }
  }
  function navigateCart() {
    navigate("/korpa");
  }

  return (
    <div>
      <div className="header">
        <button onClick={navigateLogin}>Izloguj se</button>
        <div className="shopcartDiv" onClick={navigateCart}></div>
        {brojacOrdera.number > 0 ? (
          <div className="brojacCart">
            <span>{brojacOrdera.number}</span>
          </div>
        ) : null}
      </div>

      <div className="proizvodiDiv">
        {proizvodi.map((jedan) => (
          <div className="jedanProizvodDiv" key={jedan.id}>
            <h3>{jedan.productName}</h3>
            <div className="slikaProizvod">
              <img src={require("./" + jedan.productPicture)} />
            </div>
            <h4>Cena: {jedan.productPrice}</h4>
            <h4>
              Dostupno: <span id={jedan.id}>{jedan.productQuantity}</span>
            </h4>
            <input
              onChange={(e) => {
                SetUkupnoDodano(e.target.value);
              }}
            ></input>
            <button value={jedan.id} onClick={handleOrder}>
              Dodaj u korpu
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
