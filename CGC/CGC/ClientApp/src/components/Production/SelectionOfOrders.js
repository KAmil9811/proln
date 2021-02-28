import React, { Component } from 'react';
import { OrdersTable } from './OrdersTable';
import Sidebar from '../Sidebar';
import './SelectionOfOrders.css'



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

    goback = (event) => {
        this.props.history.push('/')
    }
    goback2 = (event) => {
        this.props.history.push('/home')
    }

    render() {
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="aaaaaaaa" >
                    <div className="phone">
                        <h1>No access on the phone</h1>
                    </div>
                    <div className="HomePage">
                        <h1>Log in to have access!</h1>
                        <button type="submit" className="success_login2" onClick={this.goback} >Log in</button>
                    </div>
                </div>
            );
        }
        else {
            return (
                <div>
                    <div className="phone">
                        <h1>No access on the phone</h1>
                    </div><div className="aaaaaaaa" >


                        <div className="SelectionOfOrders">
                            <Sidebar />
                            <div className="title">
                                <h1 className="titletext">Select order</h1>
                            </div>
                            <div className="selection_of_orders_conteiner">


                                <OrdersTable />

                            </div>


                        </div>
                    </div>
                </div>
            );
        }
    }

    
}