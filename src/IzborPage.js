import React from 'react'
import { useNavigate } from 'react-router-dom'
export default function IzborPage() {
    const navigate = useNavigate();

    function navigateProdavnica(){
        navigate('/prodavnica')
    }
      return (
    <div className='LoginDiv'>
        <button className='izborbutton' onClick={navigateProdavnica}>Predjite u prodavnicu!</button>

        <button className='izborbutton'>Pogledajte transakcije na kartici!</button>
    </div>
  )
}
