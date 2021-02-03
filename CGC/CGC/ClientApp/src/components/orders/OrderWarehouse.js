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

    render() {//  let history = this.history();
        if (sessionStorage.getItem('valid') === '') {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('orderManagement') === 'true' || sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (
                <div className="OrderWarehouse">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Orders</h1>
                    </div>
                    <div className="order_warehouse_conteiner">

                        <button className="succes_add_order" onClick={this.addOrder}>Add order</button>

                        <div className="tablewar">
                            <OrderTable />
                        </div>
                    </div>
                </div>
            );
        }
        else {
            return (
                <div className="OrderWarehouse">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Orders</h1>
                    </div>
                    <div className="order_warehouse_conteiner">

                        <div className="table_corection">
                            <OrderTable />
                        </div>

                    </div>
                </div>
            );
        }
    }
}