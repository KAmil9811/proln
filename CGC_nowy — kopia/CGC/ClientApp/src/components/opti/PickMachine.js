import React, { Component } from "react";
import Sidebar from '../Sidebar';

export class PickMachine extends Component {
    displayName = PickMachine;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            machine: [],
        }
    }

    
    componentDidMount() {
        var table2 = [];
        fetch(`api/Magazine/Return_All_Colors`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        color: json[i],
                    })
                };
                this.setState({
                    machine: table2

                });
            })
    }


    cancelAdding = (event) => {
        this.props.history.push('/test')
    }

        pickMachine = (event) => {
            this.props.history.push('/selection_of_orders')
        }

    render() {
        return (

            <div>
                <Sidebar />
                <div className="AddOrder1">
                    <form>
                        <div className="form-group">
                            <label>Wybierz maszynę:</label>
                            <input
                                type="text"
                                name="client"
                                className="form-control"
                                id="inputClient"
                                
                                ref="client"
                            />
                        </div>
                        <div className="form-group">
                            <button type="button" className="cancel_order31" onClick={this.cancelAdding}>Anuluj</button>
                            <button type="button" className="then2" onClick={this.nextPage}>Wytnij</button>

                        </div>
                    </form>
                </div>

            </div>
        )
    }

}