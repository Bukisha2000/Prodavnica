import React from 'react'
import { Outlet, Navigate } from 'react-router-dom'
export default function ProtectedSecond() {
    var provera = true;
    if(localStorage.getItem('token')){
        
    }else{
        provera = false;
    }
  return (
    provera ?  <Outlet/> : <Navigate to='/'/>
  )
}
