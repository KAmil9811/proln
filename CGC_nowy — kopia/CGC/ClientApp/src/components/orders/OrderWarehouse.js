import React, { Component } from 'react';
import { OrderTable } from './OrderTable';
import './OrderWarehouse.css'
import Sidebar from '../Sidebar';


export class OrderWarehouse extends Component {
    displayName = OrderWarehouse.name;
    constructor(props) {
        super(props);
    }

    logOut = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('email')
        sessionStorage.removeItem('login')
        sessionStorage.removeItem('name')
        sessionStorage.removeItem('password')
        sessionStorage.removeItem('surname')
        sessionStorage.removeItem('user')
        sessionStorage.removeItem('admin')
        sessionStorage.removeItem('superAdmin')
        sessionStorage.removeItem('manager')
        sessionStorage.removeItem('magazineManagement')
        sessionStorage.removeItem('machineManagement')
        sessionStorage.removeItem('orderManagement')
        sessionStorage.removeItem('cutManagement')
        this.props.history.push('/')
    }


    homePage = (event) => {
        this.props.history.push('/home')
    }

    addOrder = (event) => {
        this.props.history.push('/addorderfirst')
    }



 /*   history() {
        if (sessionStorage.getItem('admin') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true') {
            return (
                <button className="add_order" onClick={this.historyOrder}>Historia zleceń</button>
            )
        }
    }*/
        historyOrder = (event) => {
            this.props.history.push('/order_history')
        }

    render() {
      //  let history = this.history();
        return (
            <div className="OrderWarehouse">
                <Sidebar />
                <div className="conteiner_ow">
                    <div className="key">
                         <h3>LEGENDA</h3>
                         <p>X- ilość sztuk, na które brakuje materiału</p>
                         <p>Y- ilość oczekujących</p>
                         <p>Z- ilość w trakcie</p>
                     </div>
                    <button className="succes_add_order" onClick={this.addOrder}>Dodaj Zlecenie</button>
                  
                    <div className="tablewar">
                        <OrderTable />
                    </div>
                </div>

                
            </div>
        );
    }
}