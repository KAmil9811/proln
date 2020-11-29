import React, { Component } from 'react';
import { OneOrderTable } from './OneOrderTable'
import './OneOrder.css'

export class OneOrder extends Component {
    displayName = OneOrder.name;
    constructor(props) {
        super(props);
    }

    homePage = (event) => {
        this.props.history.push('/home');
        sessionStorage.removeItem('orderId');
        sessionStorage.removeItem('klient');
        sessionStorage.removeItem('deadline');
        sessionStorage.removeItem('priority');
    }

    orders = (event) => {
        this.props.history.push('/orderwarehouse');
        sessionStorage.removeItem('orderId');
        sessionStorage.removeItem('klient');
        sessionStorage.removeItem('deadline');
        sessionStorage.removeItem('priority');
    }

    dataEdit = (event) => {
        //id,klient i deadline są już w pamięci
        this.props.history.push('/edit_order');

    }


    render() {
        return (
            <div>
                <div className="nav_one_o">
                    <button type="button" className="home_page5" onClick={this.homePage}>Strona główna</button>
                    <button type="button" className="orders3" onClick={this.orders}>Zlecenia</button>
                </div>
                <div className="conteiner_one_o">
                    <div className="key">
                        <p>Id zlecenia: {sessionStorage.getItem('orderId')}</p>
                        <p>Zleceniodawca: {sessionStorage.getItem('klient')}</p>
                        <p>Deadline: {sessionStorage.getItem('deadline')}</p>
<<<<<<< HEAD
                        </div>
                        


                        <div className="oneordertable">
                            <OneOrderTable />
=======
                        <p>Priorytet: {sessionStorage.getItem('priority')}</p>
                        <button type="button" className="orders3" onClick={this.dataEdit}>Edytuj dane</button>

                    </div>
                    <div className="oneordertable">
                        <OneOrderTable />
>>>>>>> 70578189a31bf14233d9ed4c81fed8f300334ed3
                    </div>
                </div>

            </div>
        );
    }
}