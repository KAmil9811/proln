import React, { Component } from "react";
import './AddOrder1.css'

export class AddOrderOne extends Component {
    displayName = AddOrderOne;
    constructor(props) {
        super(props);
        this.state = {
            value:'',
        }
    }

    nextPage = (event) => {
        const order = {
            client: this.refs.client.value,
            priority: this.refs.priority.value,
            deadline: this.refs.deadline.value,
        }
        if (this.refs.client.value === "" || this.refs.priority.value === "" || this.refs.deadline.value === "") {
            alert("Uzupełnij dane")
        }
        else {
        sessionStorage.setItem('client', order.client)
        sessionStorage.setItem('prioryty', order.priority)
        sessionStorage.setItem('deadline', order.deadline)

            this.props.history.push('/addordersecond')
        }
    }

    cancelAdding = (event) => {
        this.props.history.push('/orderwarehouse')
    }

    render() {
        return (
            <div className="AddOrder1">
                    <form> 
                    <div className="form-group">
                        <label>Klient</label>
                        <input
                            type="text"
                            name="client"
                            className="form-control"
                            id="inputClient"
                            placeholder="Wprowadź nazwę klienta"
                            ref="client"
                        />
                    </div>
                    <div className="form-group">
                        <label>Priorytet</label>
                        <input
                            type="number"
                            min="1"
                            max="5"
                            className="form-control"
                            id="inputLogin"
                            placeholder="Podaj liczbę od 1 do 5 ( 1 najwyższy priorytet)"
                            ref="priority"
                        />
                    </div>
                    <div className="form-group">
                        <label>Deadline</label>
                        <input
                            type="date"
                            className="form-control"
                            id="inputDeadline"
                            ref="deadline"
                            />
                    </div>
                    <div className="form-group">
                        <button type="button" className="cancel_order31" onClick={this.cancelAdding}>Anuluj</button>
                        <button type="button" className="then2" onClick={this.nextPage}>Dalej</button>
                       
                    </div>
                </form>
            </div>
            )
    }

}