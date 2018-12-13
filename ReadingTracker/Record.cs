using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingTracker
{

    class Record
    {
        // basics
        int m_id;
        string m_name;
        string m_book_type;
        int m_current_read;
        int m_latest_released;
        bool m_is_finished;

        // details
        string m_genre;
        string m_publisher;
        string m_category_id;
        DateTime m_initial_release_date;
        string[] m_writters;
        string[] m_artists;

        
    }
}
