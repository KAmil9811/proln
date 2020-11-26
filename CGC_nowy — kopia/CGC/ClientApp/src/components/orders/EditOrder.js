import React, { Component } from "react";
import './EditOrder.css'

export class EditOrder extends Component {
    displayName = EditOrder.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }

    handleEditOrder = (event) => {
        event.preventDefault();
        const receiver = {
            order: {
                deadline: sessionStorage.getItem('orderId'),
                owner: this.refs.klient.value,
                priority: sessionStorage.getItem('priority'),
                id_order: this.refs.deadline.value,
            },
            user: {
                login: sessionStorage.getItem('login'),
            }
        }


        fetch(`api/Order/Edit_Order`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(receiver)
                console.log(json)
                return (json)
            })
        //this.props.history.push('/oneorder');    
        // this.props.history.push('/oneorder');
    }
    cancelEditOrder = (event) => {
        //dane z cache usuwają się przy wyjściu ze zlecenia
        this.props.history.push('/oneorder');
    }
    render() {
        return (
            <div className="userChange">
                <form>
                    <div className="form-group">
                        <h2>Edycja danych</h2>
                        <label>Zleceniodawca</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputKlient"
                            placeholder={sessionStorage.getItem('klient')}
                            defaultValue={sessionStorage.getItem('klient')}
                            ref="klient"
                        />
                    </div>
                    <div className="form-group">
                        <label>Deadline</label>
                        <input
                            type="date"
                            className="form-control"
                            id="inputWidth"
                            //placeholder={sessionStorage.getItem('deadline')}
                            //defaultValue={sessionStorage.getItem('deadline')}
                            ref="deadline"
                        />
                    </div>
                    <div className="form-group">
                        <button type="submit" className="cancel_order_e" onClick={this.cancelEditOrder}>Anuluj</button>
                        <button type="submit" className="confirm_order_e" onClick={this.handleEditOrder}>Zastosuj zmiany</button>

                    </div>

                </form>
            </div>
        );
    }
}