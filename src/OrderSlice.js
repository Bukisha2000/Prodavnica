import React from 'react'
import { createSlice } from '@reduxjs/toolkit'

const initialState = {
    number: 0,
    added: [],
    myObject: {},
    totalOrders: {}
}
export const orderSlice = createSlice({
    name: "orders",
    initialState,
    reducers: {
        addOrder: (state,action) => {
            var drugaProvera = false;
            if(localStorage.getItem('order') === null || Object.keys(JSON.parse(localStorage.getItem('order'))).length == 0){
               
                state.added = [];
                state.myObject = {};
                state.totalOrders = {};
            }
            if(state.myObject[action.payload.id] != null){
               
                if((state.myObject[action.payload.id] += parseInt(action.payload.brojac)) > action.payload.stanje){
                    state.myObject[action.payload.id] -= parseInt(action.payload.brojac)
                  
                    return alert('vise nego sto ima na stanju!');
                }else{
                    
                   
                    drugaProvera = true;
                }
            }else{
                if(drugaProvera == false){
                  
                state.myObject[action.payload.id] = parseInt(action.payload.brojac);
                
                }
            }
           
          
            state.totalOrders[action.payload.id] = state.myObject[action.payload.id];
            localStorage.setItem('order',JSON.stringify(state.totalOrders));
            for(let a = 0; a<state.added.length; a++){
                 if(state.added[a] == action.payload.id){
                
                    var provera = true;
                    return;
                }
            }
            if(provera == false){
               
                state.number++;
                state.added.push(action.payload.id);
            }
            
        },
        removeOrder: (state,action) => {
           delete state.myObject[action.payload.id];
           delete state.totalOrders[action.payload.id];
        }
    }
})
export const {removeOrder} = orderSlice.actions;
export const {addOrder} = orderSlice.actions;
export default orderSlice.reducer;