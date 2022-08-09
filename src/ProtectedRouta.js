import React from 'react'
import { Outlet, Navigate } from 'react-router-dom'
export default function ProtectedRouta() {
    var provera = false;

    if(localStorage.getItem('token') !== null){
        console.log('tu sa');
        provera = true;
    }
  return (
    provera ?  <Navigate to='/prodavnica'/> : <Outlet/>
  )
}
