import React, { Component } from 'react';
import { OrdersTable } from './OrdersTable';
import Sidebar from '../Sidebar';



export class SelectionOfOrders extends Component {
    displayName = SelectionOfOrders.name;
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

    

    render() {
        return (
            <div className="OrderWarehouse">
                <Sidebar />
                <div className="conteiner_ow">
                  
                    
                    <div className="tablewar">
                        <OrdersTable />
                    </div>
                </div>


            </div>
        );
    }
}